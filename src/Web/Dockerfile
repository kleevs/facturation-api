FROM ubuntu
RUN apt update
RUN apt install wget -y
RUN wget -q https://packages.microsoft.com/config/ubuntu/19.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN apt install apt-transport-https
RUN dpkg -i packages-microsoft-prod.deb
RUN apt update
RUN apt install aspnetcore-runtime-2.1 -y
RUN mkdir /app
COPY ./* /app/
CMD cd /app && dotnet Web.dll