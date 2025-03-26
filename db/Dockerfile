FROM mysql:8.3.0
EXPOSE 3308
COPY ./EmployeeManager.Data/Migrations/ /home/database/
COPY ./EmployeeManager.Data/Datasets/ /home/database/
COPY ./EmployeeManager.Data/CI/init_database.sh /docker-entrypoint-initdb.d/init_database.sh