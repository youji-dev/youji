FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
ARG TARGETARCH
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore "Backend.sln" --arch $TARGETARCH
# Build and publish a release
RUN dotnet publish "WebApi/Application.WebApi.csproj" -c Release -o out --no-restore --arch $TARGETARCH

# Build database update bundle
RUN dotnet tool install --global dotnet-ef --version 8.0.8
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool restore
RUN dotnet ef migrations bundle --project "DataAccessLayer/PersistenceLayer.DataAccess.csproj" --startup-project "WebApi/Application.WebApi.csproj" -o /App/updateDatabase --verbose

# Build runtime image
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
ENV ASPNETCORE_HTTP_PORTS=80

RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    jq \
    libldap-2.5-0 \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

RUN if [ ! -e /usr/lib/libldap.so.2 ]; then \
        ln -s /usr/lib/x86_64-linux-gnu/libldap-2.5.so.0 /usr/lib/libldap.so.2; \
    fi && \
    if [ ! -e /usr/lib/liblber.so.2 ]; then \
        ln -s /usr/lib/x86_64-linux-gnu/liblber-2.5.so.0 /usr/lib/liblber.so.2; \
    fi

COPY --from=build-env /App/out .
COPY --from=build-env /App/updateDatabase /App/updateDatabase
COPY --from=build-env /App/docker-entrypoint.sh /App/docker-entrypoint.sh
EXPOSE 80

ENTRYPOINT [ "sh" ]
CMD ["/App/docker-entrypoint.sh"]