import { Component, OnInit, ViewEncapsulation, Injectable, Inject, ViewChild } from '@angular/core';
import { Subject } from "rxjs/Subject";
import { TagsService } from "./tags.service";
import { Observable } from 'rxjs/Observable';
import { map, takeUntil } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';
import { constants } from '../shared/constants';
import { HttpClient } from '@angular/common/http';

@Injectable()
class LocalService {
  public records: Observable<any[]>;
  private url: string = `${this._baseUrl}api/tags`;
  private _records: BehaviorSubject<any[]>;
  private dataStore: any[];

  constructor(
    @Inject(constants.BASE_URL)private _baseUrl:string,
    private _client: HttpClient) {
    this.dataStore = [];
    this._records = new BehaviorSubject([]);
    this.records = this._records.asObservable();
  }

  public getData() {
    return this._client.get(this.url)
      .subscribe((data:any) => {
        this.dataStore = data.taxes;
        this._records.next(this.dataStore);
      });
  }
}

@Injectable()
class RemoteService {
  private url: string = `${this._baseUrl}api/tags`;

  private _remoteData: BehaviorSubject<any[]>;
  public remoteData: Observable<any[]>;

  constructor(
    @Inject(constants.BASE_URL) private _baseUrl: string,
    private _client: HttpClient) {
    this._remoteData = new BehaviorSubject([]);
    this.remoteData = this._remoteData.asObservable();
  }

  public getData() {
    return this._client.get(this.url)
      .subscribe((data: any) => {
        this._remoteData.next(data.tags);
      });
  }
}

@Component({
  templateUrl: "./tag-management.component.html",
  styleUrls: ["./tag-management.component.css"],
  selector: "app-tag-management",
  providers:[LocalService, RemoteService]
})
export class TagManagementComponent {
  constructor(
    private _localService: LocalService,
    private _remoteService: RemoteService,
    private _tagsService: TagsService
  ) { }

  data;
  remote;

  public newRecord = "";

  public ngOnInit() {

    this.data = this._localService.records;
    this.remote = this._remoteService.remoteData;

    this._localService.getData();

    this._tagsService.get()
      .pipe(
        takeUntil(this.onDestroy),
        map(x => this.tags = x.tags)
      )
      .subscribe();    
  }

  editCell;

  public onInlineEdit(event) {
    this.editCell = event.cell;
  }

  tags: Array<any> = [];

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
        this.onDestroy.next();	
  }

  public handleDelete() {

  }

  public handleSave() {

  }
}
