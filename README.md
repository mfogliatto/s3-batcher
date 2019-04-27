# s3-batcher

## Context  
I'm using an AWS S3 bucket in a Google Drive fashion, using a complementary tool like ODrive to create a layer that makes it look like a regular file storing service, but with the cost difference and the flexibility to set permissions, define object lifecycles and much more.

## Problem
Although AWS provides a pretty comprehensive UI to manage your objects, this is not enough and forces you to use the aws-cli in those cases where you need a precise action to take place, or whenever this action is not even available. The specific use case that pushed me to build this was object versioning, where while you can restore specific versions, undelete and so on, you can only do this one file at a time. Therefore, if you are dealing with large amounts of objects, you don't have a quick way to do so (aws-cli even fails to work with pages -> https://github.com/aws/aws-cli/issues/3191 is still open by the time I am writing this).

## Solution
S3 Batcher allows you to execute operations like restoring and deleting objects in batch, providing a way to specify a criteria for AWS S3 to match those and apply the desired effect. Even with the bug stated above fixed, this makes it easier and extends the capabilities to work with your objects and apply predicates to determine your actual working set.

## How to use

The usage is quite simple. You basically need to provide credentials and the target resource you want to work with as the following example shows:

```shell
s3batcher-cli --access-key=MYSUPERSECRETACCESSKEY10
              --secret-key=AbCdeF1GhKijL12/jfgThI01234XyZ
              --region=us-east-1
              --operation=restore-objects
              --bucket=test-bucket
              --prefix=prefix/to/my/objects
```

### Operations supported so far are:

- Restore Objects

By providing the `--operation=restore-objects` argument, S3 Batcher will remove the delete marker of all the objects that match the specified prefix, therefore restoring the object to the latest version available.

- Delete Versions

By providing the `--operation=delete-versions` argument, S3 Batcher will delete **ALL** versions of all the objects that match the specified prefix. This means that even in a versioned bucket, the object will be completely removed, as all its versions are deleted.

### Remarks

It is important to mention that all operations can not be undone. If you want to test what the outcome of the command would be without actually doing any changes, you can add `--dryrun=true`.


You can download the latest binaries from https://sourceforge.net/projects/s3-batcher/.