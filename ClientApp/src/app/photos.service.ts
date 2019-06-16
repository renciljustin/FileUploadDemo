import { Photo } from './models';
import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PhotosService {

  constructor(private http: HttpClient) { }

  getAll() {
    return this.http.get('http://localhost:5000/api/photos')
      .pipe(
        map(res => res as Photo[])
      );
  }

  upload(file: File) {
    const form = new FormData();
    form.append('file', file);

    const req = new HttpRequest('POST', 'http://localhost:5000/api/photos', form, {
      reportProgress: true
    });

    return this.http.request(req);
  }
}
