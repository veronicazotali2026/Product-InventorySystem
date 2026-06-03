How to run the API
-------------------
- In the Product's appsettings, please set your local connection string (see DefaultConnection).
- In the solution's root directory, in order to create and seed your local SQL DB, please run the following command:
dotnet ef database update --project src\Repository\Repository.csproj --startup-project src\Products\Products.csproj
- Run Products project. Then, visit http://localhost:5112/swagger/index.html
- Things for consideration:
  1. Onion Architecture. Repository interfaces, have been seperated from implementation. This would allow to switch to any DB layers without breaking contracts.
  2. Service Manager. We bring all services, under a single service manager.
  3. API Response types.
  4. CorrelationId and integration with serilog.
  5. Resilience strategy (No more Poly)
  6. Refit for Rest.

Endpoints 
-------------------
- curl -X 'GET' \
  'http://localhost:5112/api/products/83497793-a493-4cbe-d215-08dec169d256' \
  -H 'accept: application/json'

- curl -X 'POST' \
  'http://localhost:5112/api/products' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "name": "Laptop",
  "description": "Laptop XP"
}'
