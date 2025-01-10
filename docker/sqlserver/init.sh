#!/bin/bash

# 等待 SQL Server 啟動
/opt/mssql-tools18/bin/sqlcmd -l 30 -S localhost -U sa -P $MSSQL_SA_PASSWORD -Q "SELECT 1" -C -N > /dev/null

echo "Running create-database.sql..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $MSSQL_SA_PASSWORD -i /docker-entrypoint-initdb.d/create-database.sql -C -N

echo "Running seed-data.sql..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $MSSQL_SA_PASSWORD -i /docker-entrypoint-initdb.d/seed-data.sql -C -N