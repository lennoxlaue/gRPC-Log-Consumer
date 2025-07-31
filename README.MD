# gROC Log Consumer 

A minimal gRPC-based logging server written in C#. It receives log entries from clients, stores them in a local SQLite database, and supports querying and deletion by date.

## Features

- `sendLog`: Receive logs with a timestamp and severity stage
- `getLog`: Retrieve logs by date
- `clearLog`: Delete logs by date
- Persistent storage using SQLite (`data/logs.db`)
- Includes a lightweight Python test client (not part of the repository)

## Tech Stack

- .NET 8 (C#)
- gRPC
- Entity Framework Core (EF Core)
- SQLite

## Running the Server

Make sure .NET 8 SDK is installed. Then:

```bash
dotnet restore
dotnet run
Server will run at: http://localhost:2910

gRPC Endpoints
Defined in proto/log.proto:

sendLog(sendLogRequest) returns (sendLogResponse)

getLog(getLogRequest) returns (getLogResponse)

clearLog(clearLogRequest) returns (clearLogResponse)

Python Test Client Example

send_response = stub.sendLog(log_pb2.sendLogRequest(
    log="Log entry from Python client",
    stage=log_pb2.INFO
))