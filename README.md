## Simple World grid
### About
A Unity Extension to manage big amounts of entities.
It features automatic subscription/unsubscription by radius to enable interest based messaging.
#### How to use
Implement the *IGridEntity* Interface for the entities.
Create an instance of *SimpleWorldGrid<T>* and add entities to it via  
```gridReference.Add(T entity)```
and remove them with  
```gridReference.Remove(T entity)```.
###### Note
The Grid instance currently has to be created at runtime, it can't be in a field initializer, as its update-routine is started on the parent monobehaviour.
