FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine as build
WORKDIR /root/app
COPY . .
RUN [ "dotnet", "publish", "-c", "Release" ]

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine
WORKDIR /app
COPY --from=build /root/app/bin/Release/netcoreapp3.1/publish .
RUN ["apk", "update"]
RUN ["apk", "add", "ffmpeg", "libsodium", "libsodium-dev", "opus", "opus-dev"]
ENV PREFIX="!"
ENTRYPOINT ["dotnet", "src.dll"]