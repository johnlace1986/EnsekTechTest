import { Component, OnInit } from '@angular/core';
import { MeterReadingUploaderService } from './meter-reading-uploader.service';

import { IUploadMeterReadingResult } from './upload-meter-reading-result';

@Component({
  selector: 'mru-meter-reading-uploader',
  templateUrl: './meter-reading-uploader.component.html'
})
export class MeterReadingUploaderComponent implements OnInit {

  public csvFile: File | null = null;
  public uploadMeterReadingResult: IUploadMeterReadingResult | null = null;
  public isLoading: boolean = false;

  constructor(private _service: MeterReadingUploaderService) { 
  }

  ngOnInit() {
  }

  public onCsvFileChanged(evt: any): void {
    this.csvFile = evt.target.files.item(0);
  }

  public uploadMeterReadings() {
    this.isLoading = true;

    this._service.uploadMeterReadings(this.csvFile!)
      .subscribe(result => {
        this.uploadMeterReadingResult = result;
        this.isLoading = false;
      });
  }
}