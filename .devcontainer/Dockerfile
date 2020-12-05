FROM mcr.microsoft.com/dotnet/core/sdk:3.1-focal

ARG user

RUN ["apt", "update"]
RUN ["apt", "install", "-y", "git"]
RUN ["apt", "install", "-y", "ffmpeg", "libsodium23", "libsodium-dev", "libopus0", "libopus-dev"]

RUN useradd --create-home --shell /bin/bash ${user}

USER ${user}
WORKDIR /home/{user}