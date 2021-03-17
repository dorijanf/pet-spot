import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-main-content',
  templateUrl: './main-content.component.html',
  styleUrls: ['./main-content.component.css']
})
export class MainContentComponent implements OnInit {
  mapCenter = [-122.4194, 37.7749];
  basemapType = 'satellite';
  mapZoomLevel = 12;
  constructor() { }

  ngOnInit(): void {
  }

  mapLoadedEvent(status: boolean) {
    console.log('The map loaded: ' + status);
  }

}
