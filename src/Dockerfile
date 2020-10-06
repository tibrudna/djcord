FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY . .
RUN [ "dotnet", "publish", "-c", "Release" ]

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-alpine
WORKDIR /app
COPY --from=build /src/bin/Release/netcoreapp3.1/publish .
RUN ["apk", "update"]
RUN ["apk", "add", "ffmpeg", "libsodium", "libsodium-dev", "opus", "opus-dev"]
ENV TOKEN="TOKEN"
ENV PREFIX="!"
ENTRYPOINT [ "./src" ]
