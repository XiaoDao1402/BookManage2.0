import request from '@/utils/request';
import { HistoryLogEntity } from '@/models/log';

export async function queryHistoryLog(
  params?: TableListParams,
): Promise<RequestData<HistoryLogEntity>> {
  const response: ApiModel<RequestData<HistoryLogEntity>> = await request(
    '/api/HistoryLog/QueryHistoryLog',
    {
      params,
    },
  );
  if (typeof response.value !== undefined) {
    response.value.success = response.result.success;
    return response.value;
  }
  return { success: false, data: [] };
}
