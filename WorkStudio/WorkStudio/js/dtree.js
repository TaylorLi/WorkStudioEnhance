/*--------------------------------------------------|
| dTree 2.05 | www.destroydrop.com/javascript/tree/ |
|---------------------------------------------------|
| Copyright (c) 2002-2003 Geir Landr?              |
|                                                   |
| This script can be used freely as long as all     |
| copyright messages are intact.                    |
|                                                   |
| Updated: 17.04.2003                               |
|--------------------------------------------------*/

// Node object
function Node(id, pid, name, url, title, target, icon, iconOpen, open,code,isParent,hasRead) {
	this.id = id;
	this.pid = pid;		this.code=code;//node value
	this.name = name;//html text
	this.url = url;
	this.title = title;
	this.target = target;
	this.icon = icon;
	this.iconOpen = iconOpen;
	this._io = open ;
	this._is = false;
	this._ls = false;
	this._hc = false;
	this._ai = 0;
	this._p;	this.hasRead=hasRead==1;	this.inited=false;	this.isParent=isParent==1;	this._ol=false;
};

// Tree object
function dTree(objName,object_id,is_ajax,ajax_default,ajax_url,divAllID,min_height) {
	this.config = {
		target					: null,
		folderLinks			: true,
		useSelection		: true,
		useCookies			: true,
		useLines				: true,
		useIcons				: true,
		useStatusText		: false,
		closeSameLevel	: false,
		inOrder					: false
	} 
	this.icon = {
		root				: SerUrl+'images/base.gif',
		folder			: SerUrl+'images/folder.gif',
		folderOpen	: SerUrl+'images/folderopen.gif',
		node				: SerUrl+'images/page.gif',
		empty				: SerUrl+'images/empty.gif',
		line				: SerUrl+'images/lineTree.gif',
		join				: SerUrl+'images/join.gif',
		joinBottom	: SerUrl+'images/joinbottom.gif',
		plus				: SerUrl+'images/plus.gif',
		plusBottom	: SerUrl+'images/plusbottom.gif',
		minus				: SerUrl+'images/minus.gif',
		minusBottom	: SerUrl+'images/minusbottom.gif',
		nlPlus			:SerUrl+ 'images/nolines_plus.gif',
		nlMinus			: SerUrl+'images/nolines_minus.gif'
	};
	this.obj = objName;
	this.aNodes = [];
	this.aIndent = [];
	this.root = new Node(-1);
	this.selectedNode = null;
	this.selectedFound = false;    
	this.completed = false;	this.object_id=object_id;	this.is_ajax=is_ajax;	this.ajax_default=ajax_default;	this.ajax_url=ajax_url;	this.divAllID=divAllID;	this.min_height=min_height;	if(is_ajax)	{	   SetGetAllStyle(this.obj,this.divAllID,false);	   	    a(object_id,ajax_default,'',"post",EndInitTree,true,this); 	    	}	else	{	    SetGetAllStyle(this.obj,this.divAllID,true);	}
};
//fecth the init fold
function EndInitTree(content,objId,args)
{
    var js=eval(content);
    var idIndex=0;
    for(var i=0;i<js.length;i++)
    {
        if(i==0)
            args.add(idIndex,-1,js[i].name,js[i].url,js[i].title,null,null,null,null,js[i].code,js[i].isParent,js[i].hasRead);
        else
        {
            args.add(idIndex,0,js[i].name,js[i].url,js[i].title,null,null,null,null,js[i].code,js[i].isParent,js[i].hasRead);
            idIndex++;               
        }
        idIndex++;
    }       
    document.getElementById(objId).innerHTML=args.toString();
    SetGetAllStyle(args.obj,args.divAllID,true);
    for(var i=0;i<args.aNodes.length;i++)
    {
        if(args.is_ajax && args.aNodes[i].id!=0 && args.aNodes[i]._io==true && args.aNodes[i].hasRead==false)
        {
            args.aNodes[i].hasRead=true;
            a(args.object_id,args.ajax_url+"&folder="+args.aNodes[i].code,'',"post",EndGetTree,true,[args,args.aNodes[i].id]);
            break;
        }
    }
};

// Adds a new node to the node arraydTree.prototype.add = function(id, pid, name, url, title, target, icon, iconOpen, open,code,isParent,hasRead) {    this.completed=false;	
	if(this.aNodes.length>0)this.aNodes[this.aNodes.length-1]._ls=false;
	this.aNodes[this.aNodes.length] = new Node(id, pid, name, url, title, target, icon, iconOpen, open,code,isParent,hasRead);    if(isParent&&!hasRead)
        this.aNodes[this.aNodes.length]=new Node(id+1,id,"<span class='dtree_loading'>&nbsp;&nbsp;&nbsp;</span>",'loading','',null,null,null,null,null,0,1);
};

// Open/close all nodes
dTree.prototype.openAll = function() {    
	this.oAll(true);
};
dTree.prototype.closeAll = function() {
	this.oAll(false);
};

// Outputs the tree to the page
dTree.prototype.toString = function() {    var str;     
	    str = '<div class="dtree dtree_scroll" style="max-height:'+this.min_height+'px;_height:'+this.min_height+'px;">\n';
	if (document.getElementById) {
		if (this.config.useCookies) this.selectedNode = this.getSelected();
		str += this.addNode(this.root);		
	} else str += 'Browser not supported.';
	str += '</div>';
	if (!this.selectedFound) this.selectedNode = null;
	this.completed = true;
	return str;
};
// Creates the tree structure
dTree.prototype.addNode = function(pNode) {
	var str = '';
	var n=0;
	if (this.config.inOrder) n = pNode._ai;
	for (n; n<this.aNodes.length; n++) {
		if (this.aNodes[n].pid == pNode.id) {
			var cn = this.aNodes[n];
			cn._p = pNode;
			cn._ai = n;
			this.setCS(cn);
			if (!cn.target && this.config.target) cn.target = this.config.target;
			if (cn._hc && !cn._io && this.config.useCookies) cn._io = this.isOpen(cn.id);
			if (!this.config.folderLinks && cn._hc) cn.url = null;
			if (this.config.useSelection && cn.id == this.selectedNode && !this.selectedFound) {
					cn._is = true;
					this.selectedNode = n;
					this.selectedFound = true;
			}
			str += this.node(cn, n);
			if (cn._ls) break;
		}
	}
	return str;
};

// Creates the node icon, url and text
dTree.prototype.node = function(node, nodeId) {
	var str = '<div class="dTreeNode">' + this.indent(node, nodeId);
	if (this.config.useIcons) {
		if (!node.icon) node.icon = (this.root.id == node.pid) ? this.icon.root : ((node._hc) ? this.icon.folder : this.icon.node);
		if (!node.iconOpen) node.iconOpen = (node._hc) ? this.icon.folderOpen : this.icon.node;
		if (this.root.id == node.pid) {
			node.icon = this.icon.root;
			node.iconOpen = this.icon.root;
		}
		str += '<img id="i' + this.obj + nodeId + '" src="' + ((node._io) ? node.iconOpen : node.icon) + '" alt="" />';
	}
	if (node.url) {
		str += '<a id="s' + this.obj + nodeId + '" class="' + ((this.config.useSelection) ? ((node._is ? 'nodeSel' : 'node')) : 'node') + '" href="' + node.url + '"';
		if (node.title) str += ' title="' + node.title + '"';
		if (node.target) str += ' target="' + node.target + '"';
		if (this.config.useStatusText) str += ' onmouseover="window.status=\'' + node.name + '\';return true;" onmouseout="window.status=\'\';return true;" ';
		if (this.config.useSelection && ((node._hc && this.config.folderLinks) || !node._hc))
			str += ' onclick="javascript: ' + this.obj + '.s(' + nodeId + ');"';
		str += '>';
	}
	else if ((!this.config.folderLinks || !node.url) && node._hc && node.pid != this.root.id)
		str += '<a href="javascript: ' + this.obj + '.o(' + nodeId + ');" class="node">';
	str += node.name;
	if (node.url || ((!this.config.folderLinks || !node.url) && node._hc)) str += '</a>';
	str += '</div>';
	if (node._hc) {
		str += '<div id="d' + this.obj + nodeId + '" class="clip" style="display:' + ((this.root.id == node.pid || node._io) ? 'block' : 'none') + ';">';
		str += this.addNode(node);
		str += '</div>';
	}
	this.aIndent.pop();
	return str;
};

// Adds the empty and line icons
dTree.prototype.indent = function(node, nodeId) {
	var str = '';
	if (this.root.id != node.pid) {
		for (var n=0; n<this.aIndent.length; n++)
			str += '<img src="' + ( (this.aIndent[n] == 1 && this.config.useLines) ? this.icon.line : this.icon.empty ) + '" alt="" />';
		(node._ls) ? this.aIndent.push(0) : this.aIndent.push(1);
		if (node._hc) {
			str += '<a href="javascript: ' + this.obj + '.o(' + nodeId + ');"><img id="j' + this.obj + nodeId + '" src="';
			if (!this.config.useLines) str += (node._io) ? this.icon.nlMinus : this.icon.nlPlus;
			else str += ( (node._io) ? ((node._ls && this.config.useLines) ? this.icon.minusBottom : this.icon.minus) : ((node._ls && this.config.useLines) ? this.icon.plusBottom : this.icon.plus ) );
			str += '" alt="" /></a>';
		} else str += '<img src="' + ( (this.config.useLines) ? ((node._ls) ? this.icon.joinBottom : this.icon.join ) : this.icon.empty) + '" alt="" />';
	}
	return str;
};

// Checks if a node has any children and if it is the last sibling
dTree.prototype.setCS = function(node) {
	var lastId;
	for (var n=0; n<this.aNodes.length; n++) {
		if (this.aNodes[n].pid == node.id) node._hc = true;
		if (this.aNodes[n].pid == node.pid) lastId = this.aNodes[n].id;
	}
	if (lastId==node.id) node._ls = true;
};

// Returns the selected node
dTree.prototype.getSelected = function() {
	var sn = this.getCookie('cs' + this.obj);
	return (sn) ? sn : null;
};

// Highlights the selected node
dTree.prototype.s = function(id) {
	if (!this.config.useSelection) return;
	var cn = this.aNodes[id];
	if (cn._hc && !this.config.folderLinks) return;
	if (this.selectedNode != id) {
		if (this.selectedNode || this.selectedNode==0) {
			eOld = document.getElementById("s" + this.obj + this.selectedNode);
			eOld.className = "node";
		}
		eNew = document.getElementById("s" + this.obj + id);
		eNew.className = "nodeSel";
		this.selectedNode = id;
		if (this.config.useCookies) this.setCookie('cs' + this.obj, cn.id);
	}
};

// Toggle Open or close
dTree.prototype.o = function(id) {
	var cn = this.aNodes[id];
	this.nodeStatus(!cn._io, id, cn._ls);
	cn._io = !cn._io;
	if (this.config.closeSameLevel) this.closeLevel(cn);
	if (this.config.useCookies) this.updateCookie();    if(!cn.hasRead && this.is_ajax)
    {
        SetGetAllStyle(this.obj,this.divAllID,false);
        for(var i=0;i<this.aNodes.length;i++)
        {
            if (cn.id!=this.aNodes[i].id&&!this.aNodes[i].hasRead&&this.aNodes[i]._io) {                return;//one to one load the nodes children            }
        }
        a(this.object_id,this.ajax_url+"&folder="+cn.code,'',"post",EndGetTree,true,[this,id]);         
    }
};function SetGetAllStyle(treeID,divAllID,enable) {    if (divAllID==null||divAllID==''||divAllID=='undefined') {        return;    }
    var obs=document.getElementById(divAllID).getElementsByTagName('a');
    var hrefOpen,hrefClose;    var color;    if (enable) {        hrefOpen='javascript:'+treeID+".openAll();";        hrefClose='javascript:'+treeID+".closeAll();";        color="#205AA7";    }
    else
    {
        hrefOpen='javascript:;';        hrefClose='javascript:;';        color="#BFBFBF";
    }
    for(var i=0;i<obs.length;i++)
    {
        (i==0)? obs[i].href=hrefOpen:hrefClose;            
        obs[i].style.color=color;
    }
}function EndGetTree(content,objId,args)
{
	var aNodesNew=[];	var index=0;	var dtInstance=args[0];	var pnodeid=args[1];	var newpnodeid=pnodeid;	//var ancernodeid=dtInstance.aNodes[pnodeid].pid;	var needProcess=false;	var pIndex=0;	for(var j=0;j<dtInstance.aNodes.length;j++)	{	    	    if(dtInstance.aNodes[j].url=='loading' && dtInstance.aNodes[j].pid==pnodeid)	    {	        	        needProcess=true;	        pIndex=index;	    }	    else	    {	        if(dtInstance.aNodes[j].id==pnodeid)	        {	            dtInstance.aNodes[j].hasRead=true;	            //ancernodeid=dtInstance.aNodes[i].pid;	        }	        aNodesNew[index]=dtInstance.aNodes[j];	        index++;	    }	}	var chNode=[];index=0;	if(needProcess)	{	    for(i=pIndex;i<aNodesNew.length;i++)	    {            aNodesNew[i].id--;            if (aNodesNew[i].pid>=pIndex) {                aNodesNew[i].pid--;            }        }	}		    dtInstance.aNodes=aNodesNew;    
    var js=eval(content);
    for(var i=0;i<js.length;i++)
    {//        
        dtInstance.add(dtInstance.aNodes.length,newpnodeid,js[i].name,js[i].url,js[i].title,null,null,null,null,js[i].code,js[i].isParent,js[i].hasRead);
    }
    //oo.add(oo.aNodes.length,nodeid,'Fare','javascript:GetLink(\'\',\'\')');
    //oo.add(oo.aNodes.length,nodeid,'AD','javascript:GetLink(\'AD Fare\');','');
    document.getElementById(objId).innerHTML=dtInstance.toString();
	SetGetAllStyle(dtInstance.obj,dtInstance.divAllID,true);
    for(var i=0;i<dtInstance.aNodes.length;i++)
    {
       
        if((dtInstance.is_ajax && dtInstance.aNodes[i].id!=0 && dtInstance.aNodes[i].isParent&&dtInstance.aNodes[i]._io==true && dtInstance.aNodes[i].hasRead==false) || (dtInstance._ol==true && dtInstance.aNodes[i]._io==false)||(dtInstance.aNodes[i]._io==true&&!dtInstance.aNodes[i].hasRead))
        {
            SetGetAllStyle(dtInstance.obj,dtInstance.divAllID,false);
            dtInstance.aNodes[i].hasRead=true;
            dtInstance.aNodes[i]._io=true;
            a(dtInstance.object_id,dtInstance.ajax_url+"&folder="+dtInstance.aNodes[i].code,'',"post",EndGetTree,true,[dtInstance,dtInstance.aNodes[i].id]);
            break;         
        }
        
    }    
}

// Open or close all nodes
dTree.prototype.oAll = function(status) {    if (this.is_ajax)     {        for (var n=0; n<this.aNodes.length; n++)         {
            if(status==true)
            {
                if (this.aNodes[n]._hc && this.aNodes[n].pid != this.root.id && this.aNodes[n]._io==false && !this.aNodes[n].hasRead)                 {                    this.aNodes[n].hasRead=true;
                    a(this.object_id,this.ajax_url+"&folder="+this.aNodes[n].code,'',"post",EndGetTree,true,[this,this.aNodes[n].id]);
                    this.nodeStatus(status, n, this.aNodes[n]._ls);
                    this.aNodes[n]._io = status;
                    this._ol=true;
                    break;
                }                else if (this.aNodes[n]._hc && this.aNodes[n].pid != this.root.id)                {                    this.nodeStatus(status, n, this.aNodes[n]._ls);                    this.aNodes[n]._io = status;                }            }
            else
            {
                if (this.aNodes[n]._hc && this.aNodes[n].pid != this.root.id)                 {
                    this.nodeStatus(status, n, this.aNodes[n]._ls);                    this.aNodes[n]._io = status;
                }
            }
            
        }    }
    else
    {
        for (var n=0; n<this.aNodes.length; n++) {
            if (this.aNodes[n]._hc && this.aNodes[n].pid != this.root.id) {
                this.nodeStatus(status, n, this.aNodes[n]._ls)
                this.aNodes[n]._io = status;
            }
        }       	}     if (this.config.useCookies) this.updateCookie(); 
};

// Opens the tree to a specific node
dTree.prototype.openTo = function(nId, bSelect, bFirst) {
	if (!bFirst) {
		for (var n=0; n<this.aNodes.length; n++) {
			if (this.aNodes[n].id == nId) {
				nId=n;
				break;
			}
		}
	}
	var cn=this.aNodes[nId];
	if (cn.pid==this.root.id || !cn._p) return;
	cn._io = true;
	cn._is = bSelect;
	if (this.completed && cn._hc) this.nodeStatus(true, cn._ai, cn._ls);
	if (this.completed && bSelect) this.s(cn._ai);
	else if (bSelect) this._sn=cn._ai;
	this.openTo(cn._p._ai, false, true);
};

// Closes all nodes on the same level as certain node
dTree.prototype.closeLevel = function(node) {
	for (var n=0; n<this.aNodes.length; n++) {
		if (this.aNodes[n].pid == node.pid && this.aNodes[n].id != node.id && this.aNodes[n]._hc) {
			this.nodeStatus(false, n, this.aNodes[n]._ls);
			this.aNodes[n]._io = false;
			this.closeAllChildren(this.aNodes[n]);
		}
	}
}

// Closes all children of a node
dTree.prototype.closeAllChildren = function(node) {
	for (var n=0; n<this.aNodes.length; n++) {
		if (this.aNodes[n].pid == node.id && this.aNodes[n]._hc) {
			if (this.aNodes[n]._io) this.nodeStatus(false, n, this.aNodes[n]._ls);
			this.aNodes[n]._io = false;
			this.closeAllChildren(this.aNodes[n]);		
		}
	}
}

// Change the status of a node(open or closed)
dTree.prototype.nodeStatus = function(status, id, bottom) {
	eDiv	= document.getElementById('d' + this.obj + id);
	eJoin	= document.getElementById('j' + this.obj + id);
	if (this.config.useIcons) {
		eIcon	= document.getElementById('i' + this.obj + id);
		eIcon.src = (status) ? this.aNodes[id].iconOpen : this.aNodes[id].icon;
	}
	eJoin.src = (this.config.useLines)?
	((status)?((bottom)?this.icon.minusBottom:this.icon.minus):((bottom)?this.icon.plusBottom:this.icon.plus)):
	((status)?this.icon.nlMinus:this.icon.nlPlus);
	eDiv.style.display = (status) ? 'block': 'none';
};


// [Cookie] Clears a cookie
dTree.prototype.clearCookie = function() {
	var now = new Date();
	var yesterday = new Date(now.getTime() - 1000 * 60 * 60 * 24);
	this.setCookie('co'+this.obj, 'cookieValue', yesterday);
	this.setCookie('cs'+this.obj, 'cookieValue', yesterday);
};

// [Cookie] Sets value in a cookie
dTree.prototype.setCookie = function(cookieName, cookieValue, expires, path, domain, secure) {
	document.cookie =
		escape(cookieName) + '=' + escape(cookieValue)
		+ (expires ? '; expires=' + expires.toGMTString() : '')
		+ (path ? '; path=' + path : '')
		+ (domain ? '; domain=' + domain : '')
		+ (secure ? '; secure' : '');
};

// [Cookie] Gets a value from a cookie
dTree.prototype.getCookie = function(cookieName) {
	var cookieValue = '';
	var posName = document.cookie.indexOf(escape(cookieName) + '=');
	if (posName != -1) {
		var posValue = posName + (escape(cookieName) + '=').length;
		var endPos = document.cookie.indexOf(';', posValue);
		if (endPos != -1) cookieValue = unescape(document.cookie.substring(posValue, endPos));
		else cookieValue = unescape(document.cookie.substring(posValue));
	}
	return (cookieValue);
};

// [Cookie] Returns ids of open nodes as a string
dTree.prototype.updateCookie = function() {
	var str = '';
	for (var n=0; n<this.aNodes.length; n++) {
		if (this.aNodes[n]._io && this.aNodes[n].pid != this.root.id) {
			if (str) str += '.';
			str += this.aNodes[n].id;
		}
	}
	this.setCookie('co' + this.obj, str);
};

// [Cookie] Checks if a node id is in a cookie
dTree.prototype.isOpen = function(id) {	var aOpen = this.getCookie('co' + this.obj).split('.');	for (var n=0; n<aOpen.length; n++)		if (aOpen[n] == id) return true;	return false;};

// If Push and pop is not implemented by the browser
if (!Array.prototype.push) {	Array.prototype.push = function array_push() {		for(var i=0;i<arguments.length;i++)			this[this.length]=arguments[i];		return this.length;	}};
if (!Array.prototype.pop) {	Array.prototype.pop = function array_pop() {		lastElement = this[this.length-1];		this.length = Math.max(this.length-1,0);		return lastElement;	}};

function ShowFareListTree(div_id)
{
        eval(str);
}
//Load all method
//var str=" d = new dTree('d');		d.add(0,-1,'My example tree');		d.add(1,0,'Node 12313211','example01.html');		d.add(2,0,'Node 2','example01.html');	    document.getElementById('FareListTree').innerHTML=d;";

//ajax fetch method
// var d=new dTree('d','FareListTree',true,SerUrl+"Air/CallBack.aspx?type=2",SerUrl+"Air/CallBack.aspx?type=2",'dvAllPanel',500);