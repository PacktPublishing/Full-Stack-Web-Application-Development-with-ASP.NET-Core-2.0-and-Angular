import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { constants } from "../shared/constants";

@Injectable()
export class TenantsService {
  constructor(
    @Inject(constants.BASE_URL)private _baseUrl:string,
    private _client: HttpClient    
  ) {
  }

  public verify(options: { tenantId: string }) {    
    return this._client.post(`${this._baseUrl}api/tenants/${options.tenantId}/verify`, options);
  }
  
}
