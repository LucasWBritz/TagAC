# Tag AC - Tag Access Control
A TAG Access Control microservice. 

Adding a new migration to the Management api:
dotnet ef migrations add "Initial Migration" --project src\TagAC.Management.Data.EFCore --startup-project src\TagAC.Apis.Management --output-dir Migrations 