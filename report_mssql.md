## Решения

#### Задача 1

#### Задача 2

![](./images/task2(1).png)

Для выполнения запроса будем использовать функции `MIN`, `MAX` и `MONTH`. 

```SQL

SELECT MIN(ClosePrice) as min, MAX(ClosePrice) as max, MONTH(PriceData)
        FROM dbo.tblClosePrice 
        GROUP BY MONTH(PriceData);

```

![](./images/task2(2).png)

   
 
```SQL

SELECT ClientId, Name, SUM(PaymentSum) as TotalSum FROM tblClient 
        JOIN tblClientPayments ON tblClient.ClientId = tblClientPayments.ClientId


```

2)

```SQL

SELECT 


```

#### Задача 3

#### Задача 4

#### Задача 5
