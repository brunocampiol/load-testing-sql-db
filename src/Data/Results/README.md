# Load Testing Database Results

This document provides an overview of the results obtained from the load testing conducted on the LoadTestingDb database.

**Note:** The V1 column represents durations using only `CreateSchema.sql` (no indexes from `CreateIndexes.sql`).

For a dataset of 1,000,000 (1M) rows:

## CustomerPerformanceTests.sql

| Test | Description                | V1 (ms) | V2 (ms) |
|------|----------------------------|---------|---------|
| 1    | Basic COUNT(*)             | 347     | 30      |
| 2    | Date Range Filter          | 43      | 40      |
| 3    | Name LIKE Search           | 80      | 76      |
| 4    | Group By Birth Year        | 123     | 80      |
| 5    | Complex Query              | 267     | 230     |
| 6    | TOP with ORDER BY          | 737     | 370     |
| 7    | Random Sampling            | 403     | 397     |
| 8    | GUID Lookup                | 3       | 0       |

## ProductPerformanceTests.sql

| Test | Description                | V1 (ms) | V2 (ms) |
|------|----------------------------|---------|---------|
| 1    | Basic COUNT(*)             | 100     | 26      |
| 2    | Price filtering            | 70      | 74      |
| 3    | Sorting                    | 553     | 433     |
| 4    | Aggregation                | 57      | 40      |
| 5    | Search LIKE                | 7036    | 6977    |
| 6    | Price Extremes Query       | 204     | 206     |
| 7    | Product Age Extremes Query | 200     | 187     |

## OrdersPerformanceTests.sql

| Test | Description                    | V1 (ms) | V2 (ms) |
|------|--------------------------------|---------|---------|
| 1    | Basic COUNT(*)                 | 57      | 140     |
| 2    | Date Range Filter              | 33      | 6       |
| 3    | Group By Order Month           | 87      | 100     |
| 4    | Join with Customers            | 1086    | 564     |
| 5    | Recent Orders                  | 294     | 36      |
| 6    | Random Sampling                | 6950    | 5864    |
| 7    | Order GUID Lookup              | 0       | 6       |
| 8    | Orders per Customer            | 393     | 417     |
| 9    | Complex Multi-Condition Query  | 143     | 127     |
| 10   | Daily Order Volume             | 67      | 143     |
| 11   | Customers with No Orders       | 200     | 43      |
| 12   | Customer Order Distribution    | 1057    | 3060    |

## OrderItemsPerformanceTests.sql

| Test | Description                                 | V1 (ms) | V2 (ms) |
|------|---------------------------------------------|---------|---------|
| 1    | Basic COUNT(*)                              | 737     | 130     |
| 2    | Items per Order                             | 876     | 806     |
| 3    | Product Quantity Totals                     | 1024    | 1254    |
| 4    | Complex Join                                | 10420   | 8386    |
| 5    | Order Item GUID Lookup                      | 0       | 7       |
| 6    | Order Value Calculation                     | 2430    | 2533    |
| 7    | High Quantity Orders                        | 7690    | 3507    |
| 8    | Product Distribution                        | 3427    | 3107    |
| 9    | Recent Order Items                          | 3553    | 63      |
| 10   | Last Month Items                            | 913     | 923     |
| 11   | Average Items Per Order                     | 924     | 884     |
| 12   | Customers Who Bought "Polo shirt Lace Gray M" | 240     | 230     |
| 13   | Top 10 Products with Multiple Entries Per Order | 5520    | 5167    |
| 14   | Low Stock Check                             | 1230    | 1210    |
| 15   | Customer Purchase History Pagination        | 67      | 13      |
| 16   | Frequently Bought Together                  | 17060   | 16017   |
| 17   | Customer Segmentation                       | 4120    | 3187    |
| 18   | Peak Shopping Hours                         | 800     | 726     |
| 19   | Cart Size Analysis                          | 823     | 490     |
| 20   | Product Search Simulation                   | 1624    | 1567    |
| 21   | Seasonal Trends Analysis                    | 7113    | 4857    |
| 22   | Bulk Price Recalculation                    | 857     | 550     |
| 23   | Customer Lifetime Value                     | 4486    | 3506    |
| 24   | Top Products by Order Frequency             | 4347    | 4937    |
| 25   | Products Never Ordered                      | 827     | 904     |

## Summary of Findings

- **Test Duration**: [Insert duration of tests]
- **Total Queries Executed**: [Insert total number of queries]
- **Peak Load**: [Insert peak load details]
- **Average Response Time**: [Insert average response time]
- **Maximum Response Time**: [Insert maximum response time]
- **Error Rate**: [Insert error rate]

## Metrics

- **CPU Usage**: [Insert CPU usage metrics]
- **Memory Usage**: [Insert memory usage metrics]
- **Disk I/O**: [Insert disk I/O metrics]
- **Network Latency**: [Insert network latency metrics]

## Recommendations

- [Insert recommendations based on test results]
- [Insert suggestions for optimization]
- [Insert any identified bottlenecks and proposed solutions]

## Conclusion

The load testing results indicate that the LoadTestingDb database can handle [insert conclusion based on findings]. Further testing and optimization may be required to enhance performance under extreme load conditions.