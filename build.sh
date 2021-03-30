#!/bin/bash

echo
echo "+======================"
echo "| START: MQTTProducer"
echo "+======================"
echo

source .env
echo "Using args ${BROKER} and ${CLIENTID} and ${TOPIC}"

docker build -t graboskyc/mqttproducer:latest .
docker stop mqttproducer
docker rm mqttproducer
docker run -t -i -d --name mqttproducer -e "BROKER=${BROKER}" -e "CLIENTID=${CLIENTID}"  -e "TOPIC=${TOPIC}" --restart unless-stopped graboskyc/mqttproducer:latest

echo
echo "+======================"
echo "| END: MQTTProducer"
echo "+======================"
echo
