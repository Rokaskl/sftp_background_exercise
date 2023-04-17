# SFTP Background Worker Exercise

## Task

Please develop a backend service using .Net core 6. The service should meet the
following requirements:
- Every 1 minute service connects to sftp and checks if there are new files.
- Sftp server, file paths etc. must be configurable (not in code).
- Service downloads all the new files to local path.
- All downloaded files (paths) should be stored in database (postgresql).
- Files from sftp are never deleted, so checking if file is new or old has to be done
by checking it in database taken in consideration file creation time.
- Work with database should by done by Entity framework
- Database should be defined by code first principle.
- Service should be resilient: handle connection problems etc. and should not
"die".
- Code must have comments explaining what it does.
- Service should have sane logging, configurable tracing (it should be clear what is
happening from logs).
- Service should use dependency injection.

## Solution

This project is a background worker built using .NET 6 that performs a specific task at regular intervals. The background worker is designed to connect to an SFTP server and check for new files every `jobIntervalInSeconds`. It downloads any new files to a local path and stores the file paths in a PostgreSQL database.

### Prerequisites

Before starting with this project, you need to set up the following:
- An SFTP server with a user account that has read and write access to the `remoteDirectory` and `remoteArchiveDirectory` directories.
- A PostgreSQL server and database. You should also have a user account with sufficient permissions to create tables and indexes.

### Getting started

- Clone this repository to your local machine.
- Open the solution in Visual Studio or your preferred IDE.
- Set the configuration parameters in the `appsettings.json` file.
- Open the Package Manager Console (PMC) in Visual Studio.
- In the PMC, run the command `Update-Database`.
- Build the solution and run the project.
