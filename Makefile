include .env

API = ./src/GigAuth.Api/GigAuth.Api.csproj
INFRASTRUCTURE = ./src/GigAuth.Infrastructure/GigAuth.Infrastructure.csproj

add-migration:
	dotnet ef migrations add $(name) -s $(API) -p $(INFRASTRUCTURE)

update-database:
	dotnet ef database update -s $(API) -p $(INFRASTRUCTURE)

sonar-validate:
	docker compose -f ./sonar-compose.yaml up -d
	@echo "Waiting for SonarQube to be ready..."
	@while ! curl -s http://localhost:9000/api/system/status | grep -q '"status":"UP"'; do \
		echo "Waiting for SonarQube..."; \
		sleep 5; \
	done
	@echo "SonarQube is ready!"
	dotnet sonarscanner begin /k:"gigauth" \
		/d:sonar.host.url="http://localhost:9000" \
		/d:sonar.token="${SONAR_TOKEN}" \
		/d:sonar.cs.vscoveragexml.reportsPaths="coverage.xml" \
		/d:sonar.scanner.scanAll=false
	dotnet build --no-incremental
	dotnet-coverage collect "dotnet test" -f xml -o "coverage.xml"
	dotnet sonarscanner end /d:sonar.token="${SONAR_TOKEN}"
	docker compose -f ./sonar-compose.yaml down