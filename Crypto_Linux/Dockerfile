# ベースイメージ: ARM64用の .NET 8 ランタイム
FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine-arm64 AS base
WORKDIR /app

# ビルド用イメージ
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine-arm64 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./Crypto_Linux.csproj"
RUN dotnet publish "./Crypto_Linux.csproj" -c Release -r linux-arm64 --self-contained true -o /app/publish

# 実行ステージ
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["./Crypto_Linux"]