FROM mcr.microsoft.com/dotnet/core/sdk:3.1-focal
WORKDIR /root
RUN ["apt", "update"]
RUN ["apt", "install", "-y", "git"]
RUN ["apt", "install", "-y", "ffmpeg", "libsodium23", "libsodium-dev", "libopus0", "libopus-dev"]
ENV TOKEN="TOKEN"
ENV PREFIX="!"