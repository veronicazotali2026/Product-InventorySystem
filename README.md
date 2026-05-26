How to run the API
-------------------
- Run Products project. Then, visit http://localhost:5112/swagger/index.html
- Things for consideration:
  1. Onion Architecture. Repository interfaces, have been seperated from implementation. This would allow to switch to any DB layers without breaking contracts.
  2. Service Manager. We bring all services, under a single service manager.
  3. API Response types.
  4. CorrelationId and integration with serilog.
  5. Resilience strategy (No more Poly)
  6. Refit for Rest.