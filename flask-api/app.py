from flask import Flask, request, jsonify
import pandas as pd
import joblib
from datetime import datetime

app = Flask(__name__)

# === Load Model 1 ===
ridge_model = joblib.load("ridge_model.pkl")
scaler = joblib.load("scaler.pkl")

# === Load Model 2 Forecast Data ===
forecast_df = pd.read_csv("forecast_gold_22k.csv")
forecast_df['ds'] = pd.to_datetime(forecast_df['ds'])

# === ROUTE: Model 1 - Predict Gold Price ===
@app.route('/predict', methods=['POST'])
def predict():
    data = request.json
    usd_inr = data.get("usd_inr")
    gold_usd = data.get("gold_usd")

    if usd_inr is None or gold_usd is None:
        return jsonify({"error": "Missing usd_inr or gold_usd"}), 400

    input_df = pd.DataFrame([[usd_inr, gold_usd]], columns=["USD_INR", "Gold_USD_per_ounce"])
    scaled_input = scaler.transform(input_df)
    prediction = ridge_model.predict(scaled_input)[0]

    return jsonify({"predicted_price": round(prediction, 2)})

# === ROUTE: Model 2 - Forecast Next N Days ===
@app.route('/forecast', methods=['GET'])
def forecast():
    days = int(request.args.get("days", 180))
    future_data = forecast_df.tail(days)
    future_data['ds'] = future_data['ds'].dt.strftime("%Y-%m-%d")
    result = future_data[['ds', 'yhat', 'yhat_lower', 'yhat_upper']].to_dict(orient='records')
    return jsonify(result)

# === ROUTE: Model 2 - Get Price by Date ===
@app.route('/forecast-by-date', methods=['GET'])
def forecast_by_date():
    query_date = request.args.get("date")

    if not query_date:
        return jsonify({"error": "Missing 'date' parameter"}), 400

    try:
        date_obj = datetime.strptime(query_date, "%Y-%m-%d")
        row = forecast_df[forecast_df['ds'] == date_obj]

        if row.empty:
            return jsonify({"error": "No forecast available for that date"}), 404

        return jsonify({
            "date": query_date,
            "predicted_price": round(row.iloc[0]["yhat"], 2)
        })

    except Exception as e:
        return jsonify({"error": str(e)}), 500

# === Run Flask App ===
if __name__ == '__main__':
    app.run(debug=True, port=5000)
