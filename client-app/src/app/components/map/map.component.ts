import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  Input,
  Output,
  EventEmitter,
  OnDestroy
} from "@angular/core";
import { loadModules } from "esri-loader";
import * as MapView from "esri/views/MapView";
import * as Map from "esri/Map";
import * as GraphicsLayer from "esri/layers/GraphicsLayer";
import { AnimalsService } from "src/services/animals.service";
@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit, OnDestroy {

  @Output() mapLoadedEvent = new EventEmitter<boolean>();

  // The <div> where we will place the map
  @ViewChild("mapViewNode", { static: true }) private mapViewEl: ElementRef;

  /**
   * _zoom sets map zoom
   * _center sets map center
   * _basemap sets type of map
   * _loaded provides map loaded status
   */
  private _zoom = 10;
  private _center: Array<number> = [0.1278, 51.5074];
  private _basemap = "streets";
  private _loaded = false;
  private _view: MapView = null;

  get mapLoaded(): boolean {
    return this._loaded;
  }

  @Input()
  set zoom(zoom: number) {
    this._zoom = zoom;
  }

  get zoom(): number {
    return this._zoom;
  }

  @Input()
  set center(center: Array<number>) {
    this._center = center;
  }

  get center(): Array<number> {
    return this._center;
  }

  @Input()
  set basemap(basemap: string) {
    this._basemap = basemap;
  }

  get basemap(): string {
    return this._basemap;
  }

  constructor(private animalsService: AnimalsService) { }

  async initializeMap() {
    try {
      // Load the modules for the ArcGIS API for JavaScript
      const [EsriMap, EsriMapView, Graphic, GraphicsLayer] = await loadModules([
        "esri/Map",
        "esri/views/MapView",
        "esri/Graphic",
        "esri/layers/GraphicsLayer"
      ]);

      // Configure the Map
      const mapProperties = {
        basemap: this._basemap
      };

      const map: Map = new EsriMap(mapProperties);

      // Initialize the MapView
      const mapViewProperties = {
        container: this.mapViewEl.nativeElement,
        center: this._center,
        zoom: this._zoom,
        map: map
      };

      const graphicsLayer = new GraphicsLayer();
      map.add(graphicsLayer);
      this.animalsService.getAnimals().subscribe(
        animals => {
          for (let animal of animals) {
            const point = { //Create a point
              type: "point",
              longitude: animal['location'].coordX,
              latitude: animal['location'].coordY
            };
            const simpleMarkerSymbol = {
              type: "simple-marker",
              color: [106, 27, 154],
              outline: {
                color: [255, 193, 7],
                width: 1
              }
            };
            var attributes = {
              Name: animal.name
            }
            const pointGraphic = new Graphic({
              geometry: point,
              symbol: simpleMarkerSymbol,
              attributes: attributes
            });

            graphicsLayer.add(pointGraphic);
          }
        }
      )
      this._view = new EsriMapView(mapViewProperties);
      await this._view.when();
      return this._view;
    } catch (error) {
      console.log("EsriLoader: ", error);
    }
  }

  ngOnInit() {
    // Initialize MapView and return an instance of MapView
    this.initializeMap().then(mapView => {
      // The map has been initialized
      console.log("mapView ready: ", this._view.ready);
      this._loaded = this._view.ready;
      this.mapLoadedEvent.emit(true);
    });
  }

  ngOnDestroy() {
    if (this._view) {
      // destroy the map view
      this._view.container = null;
    }
  }
}
