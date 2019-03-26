
FROM microsoft/dotnet:2.1-aspnetcore-runtime
COPY ./pub/ /root/
WORKDIR /root/
ENV ASPNETCORE_URLS="http://*:80"
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "BasketApp.dll"]