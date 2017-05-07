# YASC
Yet Another SSL Checker

## Introduction
YASC is a simple utility to check the health of SSL certificates for websites. It boasts the following features:

1. Live testing of an SSL certificate
2. Scheduled testing and batch emails of 1 or more SSL certificates

## Technical Notes
YASC is a technology playground for me. It makes use of bootstrap 4 (alpha 6 currently) and also is an asp.net core MVC web application. It makes a (small amount) of use of 
entity framework core. In order to handle background processing, it uses the excellent [hangfire.io](http://hangfire.io) library. It also makes use of [SendGrid](http://www.sendgrid.com)'s transaction email templates.

# DANGER WILL ROBINSON
While I have this running in Azure, it is on an Azure free plan. THE APPLICATION WILL STOP! There is a nearly 100% chance that alerts will NOT go out all of the time. DO NOT RELY ON IT! (You have been warned in lots of capital letters).
