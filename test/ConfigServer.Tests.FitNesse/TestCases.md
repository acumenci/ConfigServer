# Tests

## Can get config
* Sense check that we are able to get some config out.
 - Stand up a config server
 - Set it up with a GDrinx client that has a Test environment
 - Do a GET on /GDrinx/Test
 - Assert that all properties from this client/environment are in the returned object

## Config is different for different clients
* Try some different clients and see that the config is different.
* Try some different environments and see that the config is different.

## Can edit config
* Changes take effect in the proper scope:
 * Get a bit of config
 * Change it
 * See that it has changed
* Changes don't take effect outside of scope:
 * Get a bit of config
 * Change it
 * See that nothing else has changed elsewhere

## Can transfer config
* Promote some config to a different environment. See that it is in that environment.
* Attempt to transfer config accross clients. Check that that is not allowed.



## Requesting configs that don't exist
* Server gives an error explaining that the config does not exist.

## Requesting configs that do exist but not for the current client/environment
* Server gives an error explaining that this configuration is not available for the requested client/environment.

## Client Library?

## Authentication
* Users that are not authenticated can't view config.
* Users that are not authenticated can't change config.
