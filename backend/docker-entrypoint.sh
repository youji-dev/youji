#!/bin/bash

set -e

echo "Updating database..."
APPSETTINGS_PATH="/App/appsettings.json"
DB_CONNECTION_STRING=$(jq -r '.ConnectionStrings.DefaultConnection' $APPSETTINGS_PATH)
echo "Connection string: $DB_CONNECTION_STRING"
/App/updateDatabase --connection $DB_CONNECTION_STRING

echo "Starting application..."
exec dotnet Application.WebApi.dll