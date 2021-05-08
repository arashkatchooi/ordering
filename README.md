# ordering
Ordering System

This solution consist of three projects written in dotnet core c#:
1. Order.Supervisor :  hosts the api end points
2. Order.Agent: host agent code
3. Order.Common: contains share code including models and services

# Supervisor
Supervisor post end point is :https://localhost:44361/order/request
1.Post endpoint accepts one command as string.
2.Creates an order item.
3.Serializes the order item.
4.Pushes order item into the azure storage emulator queue.

# Agent
Agent is a windows console application which listens to messages comming from azure storage emulator.

# Services
1. Queue service is injected to both Supervisor and Agent and has queue and dequeue methods.

