import { Tag } from "../tags/tag.model";

export class Note { 
  public noteId: any = 0;
  public title: string;
  public body: string;
  public tags?: Array<Tag> = [];
}
