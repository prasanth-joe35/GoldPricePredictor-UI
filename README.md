Gold Price Predictor (22K INR/Gram)
A full-stack project to predict gold prices using machine learning. It includes a user-friendly .NET MVC web app (frontend) and a Python Flask API (backend) with two models:

Model 1 – Predict gold rate per gram based on USD-INR rate and ounce price

Model 2 – Forecast gold price trend for the next 1 year and check price on any future date

📌 Project Overview
Component	Technology Used
UI Frontend	ASP.NET MVC
Backend API	Flask (Python)
ML Model 1	Ridge Regression
ML Model 2	Prophet Forecasting
Data Source	yFinance (Live Data)
Charts	Plotly + Bootstrap
📂 Folder Structure
csharp
Copy
GoldPricePredictor-UI/
├── GoldPricePredictor-UI/        → .NET MVC frontend
├── flask-api/                    → Flask backend with ML models
│   ├── app.py                    → API routes
│   ├── ridge_model.pkl           → Model 1 (What-if)
│   ├── scaler.pkl                → Model 1 scaler
│   ├── forecast_gold_22k.csv     → Forecasted prices (Model 2)
│   ├── gold_22k_usd_inr_data.csv → Cleaned dataset
│   └── requirements.txt          → Python packages
├── README.md                     → This file
✨ Features
🔢 Model 1: What-If Simulation
Input: USD to INR and Gold per Ounce

Output: 22K Gold Rate (₹/gram)

✅ Great for “if this, then what” predictions

📈 Model 2: Forecast Trend
Shows upcoming gold price trend (6 to 12 months)

Users can visually see the rise/fall based on market

📅 Price by Date
Enter any future date to get the predicted gold price

🚀 How to Run the Project
✅ Step 1: Start Flask API
bash
Copy
cd flask-api
pip install -r requirements.txt
python app.py
Runs at: http://127.0.0.1:5000/

✅ Step 2: Start .NET MVC App
Open GoldPricePredictor-UI in Visual Studio or VS Code

Run the project

Runs at: https://localhost:PORT/Gold/Index

Author
Created by [Prasanth.A]
This mini-project shows my skills in Machine Learning, API integration, and full-stack web development.

📄 License
MIT License — free to use and modify

