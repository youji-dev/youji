FROM node:22.13-slim AS build

WORKDIR /build

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build

FROM node:22.13-slim AS base

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
