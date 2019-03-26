# Checkout Basket

To run the app use command 'dotnet run' in the BasketApp directory.
The application is available via http://localhost:5001.

## General architecture summary:

Rest Web API with the following endpoints:

**GET** api/basket

**POST** api/basket 
{
 "ProductId" : string,
 "Quantity" : int
}

**PUT** api/basket
{
 "ProductId" : string,
 "Quantity" : int
}

**DELETE** api/basket


The PUT and POST become interchangeable in this implementation because of the SaveOrUpdate approach in the store layer.

The identifier of the basket is based very basically on a Session cookie assumed to have been set and supplied by the provider of the request.
A more complex implementation could create and set the cookie if it doesn't already exist and control its TTL. Stronger authentication could be 
used like JWT tokens depending on how secure this basket data needs to be.

A Controller/Service/Repository pattern has been used. The store layer is being stubbed very lightly in memory but the general idea would be that
this could be a Redis store, where the data would never hang around for too long because the TTL could be controlled.

I've implemented a light retry pattern on the service layer. This is using generic exception catching and wait pattern, this could be responding
to specific errors from the data store, and deciding to retry depending on the type. It could also use a fallback pattern depending on error types.

## Things missing:
* Validation. Ideally adding to a basket would use a service to check the existance of the product and validate whether its possible to add to the 
basket. The idea would be that this would be in the basket service layer and the response errors could be expanded to include validation errors 
and not just errors relating to storing the basket item.
* Config driven values
* Swagger

