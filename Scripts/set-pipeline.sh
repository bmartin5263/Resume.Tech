#!/bin/bash -e

yes | fly -t resumetech set-pipeline -c Cicd/pipeline.yml -p resumetech -l Cicd/variables.yml