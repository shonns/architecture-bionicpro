# Используем официальный образ .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Копируем csproj и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем остальные файлы и собираем приложение
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Используем официальный рантайм-образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build /app/publish .

# Открываем порт (замените на ваш, если не 8000)
EXPOSE 8000

# Явно указываем ASP.NET Core слушать порт 8000 (замените на ваш, если не 8000)
ENV ASPNETCORE_URLS=http://+:8000

# Запуск приложения
ENTRYPOINT ["dotnet", "api.dll"]
