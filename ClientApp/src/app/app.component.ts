import { PhotosService } from './photos.service';
import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { Photo, Progress } from './models';
import { HttpEventType, HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  @ViewChild('fileInput') fileInput: ElementRef;
  photos: Photo[];
  progress: Progress = {
    total: 0,
    loaded: 0,
    percentage: 0,
  };

  constructor(private photosService: PhotosService) {

  }

  ngOnInit() {
    this.photosService.getAll()
      .subscribe(
        photos => {
          this.photos = photos;
          console.table(this.photos);
        },
        error => {
          console.log(error);
        }
      );
  }

  onUpload() {
    const file: File = this.fileInput.nativeElement.files[0];

    this.photosService.upload(file)
      .subscribe(
        event => {
          if (event.type === HttpEventType.UploadProgress) {
            this.progress.total = event.total;
            this.progress.loaded = event.loaded;
            this.progress.percentage = Math.round(100 * this.progress.loaded / this.progress.total);
            console.log(this.progress.percentage + '%');
          } else if (event instanceof HttpResponse) {
            this.progress = {
              total: 0,
              loaded: 0,
              percentage: 0
            };
            this.photos.push(event.body as Photo);
            console.log('Upload Complete.');
          }
        },
        error => {
          console.error(error);
          this.progress = {
            total: 0,
            loaded: 0,
            percentage: 0
          };
        }
      );
}
}
