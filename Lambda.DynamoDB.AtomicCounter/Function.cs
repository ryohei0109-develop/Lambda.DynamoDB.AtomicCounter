using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Lambda.DynamoDB.AtomicCounter.Model;
using System;
using System.Collections.Generic;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Lambda.DynamoDB.AtomicCounter
{
    public class Function
    {
        /// <summary>
        /// DynamoDB�̃A�g�~�b�N�J�E���^�[�X�V�������s���܂��B
        /// </summary>
        /// <param name="atomicCounterRequestModel"></param>
        /// <param name="context"></param>
        /// <returns>�X�V��̒l��Ԃ��܂��B</returns>
        public string FunctionHandler(AtomicCounterRequestModel atomicCounterRequestModel, ILambdaContext context)
        {
            string afterVersion = null;

            using (AmazonDynamoDBClient client = new AmazonDynamoDBClient(
                atomicCounterRequestModel.AccessKey,
                atomicCounterRequestModel.SecretKey,
                atomicCounterRequestModel.EndPoint))
            {
                try
                {
                    UpdateItemRequest updateItemRequest = new UpdateItemRequest
                    {
                        TableName = atomicCounterRequestModel.TableName,
                        Key = new Dictionary<string, AttributeValue>()
                        {
                            {
                                atomicCounterRequestModel.HashName, new AttributeValue()
                                {
                                    S = atomicCounterRequestModel.HashValue
                                }
                            }
                        },
                        ExpressionAttributeNames = new Dictionary<string, string>()
                        {
                            { "#v", atomicCounterRequestModel.CounterFieldName }
                        },
                        ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                        {
                            { ":incr", new AttributeValue()
                                {
                                    N = "1"
                                }
                            }
                        },
                        UpdateExpression = "SET #v = #v + :incr",
                        ReturnValues = "UPDATED_NEW"
                    };

                    afterVersion = client.UpdateItemAsync(updateItemRequest)
                        .Result.Attributes[atomicCounterRequestModel.CounterFieldName].N;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return afterVersion;
        }
    }
}
