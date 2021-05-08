# ordering
Ordering System

This solution consist of three projects:
a) Order.Supervisor :  hosts the api end points
b) Order.Agent: host agent code
c) Order.Common: contains share code including models and services

How Program works:
1- Post end point is :https://localhost:44361/order/request
-post endpoint is accepting one command as string 
-creates an order item
-serialize the order item
-push it to azure storage emulator queue

2- Agent is windows console application which has infinit loop and gets message from azure storage emulator every 2 seconds
