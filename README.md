# s3-batcher

## Context  
I'm using an AWS S3 bucket in a Google Drive fashion, using a complementary tool like ODrive to create a layer that makes it look like a regular file storing service, but with the cost difference and the flexibility to set permissions, define object lifecycles and much more.

## Problem
Although AWS provides a pretty comprehensive UI to manage your objects, this is not enough and forces you to use the aws-cli in those cases where you need a precise action to take place, or whenever this action is not even available. The specific case that pushed me to build this was object versioning, where while you can (from the UI and with aws-cli) restore specific versions, undelete (remove restore markers) and so on, you can just do this on a one-file-per-time basis. So if you are dealing with large amounts of objects, you don't have a quick way to do so (aws-cli even fails to work with pages -> https://github.com/aws/aws-cli/issues/3191 is still open by the time of the writing).

## Solution
S3 Batcher allows you to execute a number of operations like restoring previous versions to multiple objects in a single operation, providing a way to specify a criteria for AWS S3 to match those and apply the desired effect. Even with the bug stated above fixed, this makes it easier and extends the capabilities to work with your objects and apply predicates to determine your actual working set.