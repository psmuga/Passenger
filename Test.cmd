@echo off
set projects=(Passenger.Tests Passenger.Tests.EndToEnd)
for %%i in %projects% do (
	echo Running tests for %%i
	dotnet test %%i/%%i.csproj
)