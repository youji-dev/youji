FROM node:20.16.0-slim AS build

WORKDIR /build

COPY package*.json ./
RUN npm install
# Install dependencies for building on linux
RUN npm install sass-embedded-linux-x64

COPY . .
RUN npm run build

FROM node:20.16.0-slim AS base

ARG DEBIAN_FRONTEND=noninteractive
ENV TZ=Europe/Berlin

RUN apt -y update
RUN apt-get install -y openssl
RUN apt-get clean && rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

FROM base
WORKDIR /build
COPY --from=build /build/.output/ .
EXPOSE 3000
CMD [ "node", "server/index.mjs" ]