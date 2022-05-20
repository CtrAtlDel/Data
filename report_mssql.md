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

   
1. 

```SQL

SELECT ClientId, Name, SUM(PaymentSum) as TotalSum FROM tblClient 
        JOIN tblClientPayments ON tblClient.ClientId = tblClientPayments.ClientId


```

2.
В задаче будем использовать PostgreSQL
```SQL

SELECT ClientId, Name, SUM(PaymentSum) as TotalSum FROM tblClient 
        JOIN tblClientPayments ON tblClient.ClientId = tblClientPayments.ClientId
        WHERE TotalSum > 7000 OR PaymentDate > '05.03.2022'


```
3.
```SQL ???

SELECT ClientId, Name FROM tblClient 
        JOIN tblClientPayments ON tblClient.ClientId = tblClientPayments.ClientId
        WHERE 

```
4.
```SQL
UPDATE tblClientPayments SET PaymentSum = PaymentSum * 0.9 WHERE PaymentData = '02.03.2022' AND ClientId = 3
```

```SQL
INSERT INTO tblClientPayments(ClientId, PaymentDate, PaymentSum) VALUES (3, NOW(), 18000);
```

#### Задача 3
Используем POstgreSQL
```sql

SELECT dbo.tblClosePrice.PriceDate as dbo.tblClosePrice.DateT, dbo.tblClosePrice.PriceAssetId as AssetId, 
        dbo.tblClosePrice.ClosePrice as PriceT, T.ClosePrice as PriceT1 FROM dbo.tblClosePrice
        JOIN dbo.tblClosePrice T ON dbo.tblClosePrice.PriceAssetId = T.PriceAssetId WHERE T.ClosePrice IS NOT NULL AND T.PriceDate < dbo.tblClosePrice.PriceDate AND (dbo.tblClosePrice.ClosePrice =  T.ClosePrice * 0.30)

```

#### Задача 4


#### Задача 5

