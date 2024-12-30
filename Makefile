include .env

API = ./src/GigAuth.Api/GigAuth.Api.csproj
INFRASTRUCTURE = ./src/GigAuth.Infrastructure/GigAuth.Infrastructure.csproj

add-migration:
	dotnet ef migrations add $(name) -s $(API) -p $(INFRASTRUCTURE)

update-database:
	dotnet ef database update -s $(API) -p $(INFRASTRUCTURE)