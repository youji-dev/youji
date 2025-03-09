<!-- Für eine angenehmere Darstellung dieser Datei kann https://github.com/youji-dev/youji aufgerufen werden -->

![Youji Banner Dark](./resources/ReadmeBannerDark.png#gh-dark-mode-only)
![Youji Banner Light](./resources/ReadmeBannerLight.png#gh-light-mode-only)

# Youji

Youji ist das das Hausmeister Ticketsystem der Industrieschule Chemnitz

## Deploy als Produktumgebung

Das Deployment von Youji erfolgt mithilfe von Docker. Im Verzeichnis deployment befinden sich:

Eine docker-compose.yml-Datei, die alle notwendigen Container definiert.
Zusätzliche Konfigurationsdateien, z. B. für die Backend-Einstellungen.
Um Youji zu starten, navigiere in das deployment-Verzeichnis und führe den folgenden Befehl aus:

```sh
docker compose up -d
```

## Docker Stack

### Die docker-compose.yml-Datei erstellt die folgenden Container:

1. Datenbank (Youji_Database)

- PostgreSQL-Datenbank
- Speichert persistente Daten in einem Docker-Volume
- Bei Bedarf kann eine eigene PostgresSQL Datenbank verwendet werden. Dafür muss dieser Service aus der docker-compose.yml entfernt werden und der Datenbank Connection String in der

2. Backend (Youji-Backend)

- Backend-Anwendung von Youji
- Abhängig von der Datenbank. Startet erst sobald die Datenbank bereit und initialisiert ist.
  - Erstellt eigenständig die vollständige Datenbankstruktur oder updated diese bei bedarf
- Läuft unter Port 3001
- Konfigurationsdatei (appsettings.json) wird per Volume eingebunden

3. Frontend (Youji-Frontend)

- Web-Oberfläche von Youji
- Abhängig vom Backend. Startet erst sobald das Backend startet
- Läuft unter Port 3000
- Über die Umgebungsvariable NUXT_PUBLIC_BACKEND_URL(.env) wird bestimmt, über welche öffentlich Domain das Backend erreichbar ist.

### Konfigurationen

In den folgenden Dateien des `deployment` Verzeichnisses müssen folgende Konfigurationen festgelegt werden:

#### default.env

> [!WARNING]
> Diese Datei muss zu `.env` umbenannt werden

| Name               | Beschreibung                                                             | Beispiel              |
| ------------------ | ------------------------------------------------------------------------ | --------------------- |
| POSTGRES_USER      | Benutzername für die Datenbank (Nach dem ersten Start nicht veränderbar) | youji                 |
| POSTGRES_PASSWORD  | Passwort für die Datenbank (Nach dem ersten Start nicht veränderbar)     | mysecurepassword      |
| POSTGRES_DB        | Name der Datenbank (Nach dem ersten Start nicht veränderbar)             | youji_db              |
| PUBLIC_BACKEND_URL | Öffentliche URL des Backends                                             | http://localhost:3001 |

#### backend.appsettings.json

| Name                                  | Beschreibung                                                                                                                   | Datentyp | Beispiel                                                                 |
| ------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------ | -------- | ------------------------------------------------------------------------ |
| Logging.LogLevel.Default              | Standard-Log-Level                                                                                                             | string   | "Information"                                                            |
| Logging.LogLevel.Microsoft.AspNetCore | Log-Level für ASP.NET Core                                                                                                     | string   | "Warning"                                                                |
| ConnectionStrings.DefaultConnection   | Verbindungs-String zur Datenbank                                                                                               | string   | "Host=database;Database=youji_db;Username=youji;Password=secret"         |
| DevAuth                               | Entwicklungsmodus für Authentifizierung                                                                                        | bool     | false                                                                    |
| JWTKey                                | Schlüssel für JWT-Signatur                                                                                                     | string   | "my-secret-jwt-key"                                                      |
| SessionLifeTime                       | Lebensdauer der Sitzung in Minuten. Kann mit refresh-token verlängert werden                                                   | int      | 720                                                                      |
| LDAP.Host                             | Host des LDAP-Servers                                                                                                          | string   | "ldap.example.com"                                                       |
| LDAP.Port                             | Port des LDAP-Servers                                                                                                          | int      | 389                                                                      |
| LDAP.BaseDN                           | Basis-DN für LDAP-Suche                                                                                                        | string   | "dc=example,dc=com"                                                      |
| LDAP.EMailAttributeName               | Attribut für die E-Mail-Adresse im LDAP                                                                                        | string   | "userPrincipalName"                                                      |
| CORS.AllowedOrigins                   | Liste der erlaubten Client Origins (CORS)                                                                                      | string[] | ["http://localhost:3000"]                                                |
| CORS.AllowedHeaders                   | Erlaubte HTTP-Header (CORS)                                                                                                    | string[] | ["Authorization", "content-type"] (Diese sind zwingend benötigt)         |
| CORS.AllowedMethods                   | Erlaubte HTTP-Methoden (CORS)                                                                                                  | string[] | ["GET", "POST", "PUT", "PATCH", "DELETE"] (Diese sind zwingend benötigt) |
| Mail.SenderName                       | Anzeigename des Absenders                                                                                                      | string   | "Youji Hausmeister Ticketsystem"                                         |
| Mail.SenderAddress                    | E-Mail-Adresse des Absenders                                                                                                   | string   | "no-repy@youji.com"                                                      |
| Mail.SmtpAddress                      | Adresse des SMTP-Servers                                                                                                       | string   | "smtp.youji.com"                                                         |
| Mail.SmtpPort                         | Port des SMTP-Servers                                                                                                          | int      | 587                                                                      |
| Mail.UseSsl                           | Ob SSL für SMTP verwendet wird                                                                                                 | bool     | true                                                                     |
| Mail.SubjectFormat                    | Format des E-Mail-Betreffs                                                                                                     | string   | "[Youji] {0}"                                                            |
| Mail.SmtpUser                         | Benutzername für SMTP-Login                                                                                                    | string?  | "youji-smtp-user"                                                        |
| Mail.SmtpPassword                     | Passwort für SMTP-Login                                                                                                        | string?  | "supersecretpassword"                                                    |
| Images.UnrenderableMimeTypes          | Nicht darstellbare Bildformate. Für diese Formate wird kein BlurHash generiert und im Frontend keine Bilddarstellung angezeigt | string   | "image/svg+xml"                                                          |

> [!CAUTION]
> Der Wert `DevAuth` sollte in Produktiv Umgebungen **IMMER** auf `false` sein. Ist dieser aktiv werden Login Mechaniken effektiv ausgehebelt. Dann ist ein Login mit einem beliebigen Nutzer und Passwort immer möglich. Nicht bekannte Nutzer werden angelegt

> [!TIP]
> Für die Generierung des `JWTKey` kann [jwtsecret](https://jwtsecret.com/generate) verwendet werden.

> [!TIP]
> In dem Wert `Mail.SubjectFormat` wird `{0}` mit dem einem Betreff Text ersetzt. Aus "[Youji] {0}" wird z.B "[Youji] New comment on ticket 'Defekte Heizung im Keller'"

> [!TIP]
> Wenn `Mail.SmtpUser` und `Mail.SmtpPassword` beide `null` sind, wird sich ohne Authentifizierung an dem SMTP Server angemeldet
