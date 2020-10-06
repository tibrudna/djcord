FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY . .
RUN [ "dotnet", "publish" ]

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine
WORKDIR /app
COPY --from=build /src/bin/Debug/netcoreapp3.1/publish .
RUN ["apk", "update"]
RUN ["apk", "add", "ffmpeg", "libsodium", "libsodium-dev", "opus", "opus-dev"]
ENTRYPOINT [ "./src" ]