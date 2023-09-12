import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { IUploadMeterReadingResult } from './upload-meter-reading-result';

@Injectable({
  providedIn: 'root'
})
export class MeterReadingUploaderService {
    private _baseAddress: string = 'https://localhost:7264/'

    constructor(private _client: HttpClient) {  
    }

    uploadMeterReadings(csvFile : File): Observable<IUploadMeterReadingResult> {
        const formData = new FormData();
        formData.append('file', csvFile);

        const headers = new HttpHeaders({ 'enctype': 'multipart/form-data' });

        return this._client.post<IUploadMeterReadingResult>(this._baseAddress + "meter-reading-uploads", formData, { headers: headers });
    }
}