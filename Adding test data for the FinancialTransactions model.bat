curl -X POST -H "Content-Type: application/json;" -d "{\"description\": \"Received salary according to the contract\",\"amount\": 8000,\"date\": \"2023-06-01\",\"typeId\": 1}" https://localhost:7213/api/FinancialTransactions/
curl -X POST -H "Content-Type: application/json;" -d "{\"description\": \"Paying for car insurance\",\"amount\": -500,\"date\": \"2023-06-01\",\"typeId\": 6}" https://localhost:7213/api/FinancialTransactions/
curl -X POST -H "Content-Type: application/json;" -d "{\"description\": \"Income from Tesla shares\",\"amount\": 1500,\"date\": \"2023-06-05\",\"typeId\": 2}" https://localhost:7213/api/FinancialTransactions/
curl -X POST -H "Content-Type: application/json;" -d "{\"description\": \"Payment of interest on a bank deposit\",\"amount\": 300,\"date\": \"2023-06-05\",\"typeId\": 3}" https://localhost:7213/api/FinancialTransactions/
curl -X POST -H "Content-Type: application/json;" -d "{\"description\": \"Grocery shopping at Walmart\",\"amount\": -2300,\"date\": \"2023-06-05\",\"typeId\": 4}" https://localhost:7213/api/FinancialTransactions/
curl -X POST -H "Content-Type: application/json;" -d "{\"description\": \"Dental services\",\"amount\": -1500,\"date\": \"2023-06-06\",\"typeId\": 5}" https://localhost:7213/api/FinancialTransactions/
curl -X POST -H "Content-Type: application/json;" -d "{\"description\": \"Rest in the park\",\"amount\": -700,\"date\": \"2023-06-07\",\"typeId\": 7}" https://localhost:7213/api/FinancialTransactions/