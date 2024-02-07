
## start docker service
Start-Process powershell -Verb runAs -ArgumentList 'Start-Service com.docker.service'

## docker commands
- cd C:\Users\lenovo\Documents\GitHub\AstTrader\scripts\
- docker-compose up -d
- - not required -- docker build -f mongodb-setup.dockerfile -t mongodb-local .


## docker compose vs docker run
- run: used to start single service, specify all options on command line
- compose: better, start services in one file, configuration/options in yml file 
- stauts: docker-compose up / ps / top / stop < service-name > / down / down -v
