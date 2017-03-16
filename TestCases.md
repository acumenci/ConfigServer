# Tests

## Can get config

* Sense check that we get some shit out.

## Config is different for different clients

* Try some different clients and see that the config is different.
* Try some diffeerent environments see that config is different.

## Can edit config.

* Get a bit of config. Change it. See that it has changed
* Get a bit of config. Change it. See that it has not changed elsewhere.

## Can transfer config.

* Promote some config to a different environment. See that it is in that environment.
* Attempt to transfer config accross clients. Check that that is not allowed.




# Requesting configs that don't exist.

* Server gives an error explaining that the config does not exist.

# Requesting configs that do exist but not for the current client/environment

* Server gives an error explaining that this configration is not available for the requested client/environemnt


## Client Library?


## Authentication

* Users that are not authenticated can't view shit.
* Users that are not authenticated can't change shit.